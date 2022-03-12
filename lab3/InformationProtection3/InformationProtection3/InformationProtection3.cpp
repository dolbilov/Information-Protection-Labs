#include <iostream>
#include <fstream>
#include <Windows.h>

using namespace std;

void hide_byte_into_pixel(RGBQUAD* pixel, uint8_t hideByte) {
	pixel->rgbBlue &= (0xFC);
	pixel->rgbBlue |= (hideByte >> 6) & 0x3;
	pixel->rgbGreen &= (0xFC);
	pixel->rgbGreen |= (hideByte >> 4) & 0x3;
	pixel->rgbRed &= (0xFC);
	pixel->rgbRed |= (hideByte >> 2) & 0x3;
	pixel->rgbReserved &= (0xFC);
	pixel->rgbReserved |= (hideByte) & 0x3;
}

uint8_t decode_byte_from_pixel(RGBQUAD pixel) {
	uint8_t hideByte = pixel.rgbBlue & 0x3;
	hideByte = hideByte << 2;
	hideByte |= pixel.rgbGreen & 0x3;
	hideByte = hideByte << 2;
	hideByte |= pixel.rgbRed & 0x3;
	hideByte = hideByte << 2;
	hideByte |= pixel.rgbReserved & 0x3;
	return hideByte;
}

template <typename T>
void read(ifstream& fp, T& result) {
	fp.read(reinterpret_cast<char*>(&result), sizeof(result));
}

template <typename T>
void write(ofstream& fp, T text) {
	fp.write(reinterpret_cast<char*>(&text), sizeof(text));
}

int main() {
	BITMAPFILEHEADER fileHeader; 
	BITMAPINFOHEADER infoHeader;
	RGBQUAD pixel;
	
	//string fileToDecode = "image.bmp";
	string fileToDecode = "newPicture.bmp";

	ifstream picture(fileToDecode, ios_base::in | ios_base::binary);
	if (!picture) {
		cout << "Can't read file" << endl;
		return -1;
	}

	read(picture, fileHeader);
	read(picture, infoHeader);

	cout << "What do you want to do?" << endl <<
		"1 - Decode text from bmp" << endl <<
		"2 - Encode text to bmp" << endl;

	int command;
	cin >> command;

	if (command == 1) {
		// decoding text from bmp
		ofstream text("decoded text.txt", ios_base::out | ios_base::binary);

		while(true) {
			read(picture, pixel);
			auto byte = decode_byte_from_pixel(pixel);
			if (byte == 0xFF) {
				break;
			}
			text << byte;
		}


		text.close();
	}
	else if (command == 2) {
		// encoding text to bmp
		ifstream text("text to encode.txt", ios_base::in | ios_base::binary);
		ofstream newPicture("newPicture.bmp", ios_base::out | ios_base::binary);

		write(newPicture, fileHeader);
		write(newPicture, infoHeader);
		int writtedPixels = 0; 
		while (true) {
			uint8_t symbol;
			read(text, symbol);
			if (text.eof()) {
				break;
			}
			read(picture, pixel);
			hide_byte_into_pixel(&pixel, symbol);
			write(newPicture, pixel);
			writtedPixels++;
		}

		read(picture, pixel);
		hide_byte_into_pixel(&pixel, 0xFF);
		write(newPicture, pixel); // write EOF
		writtedPixels++;

		for (int i = writtedPixels; i < infoHeader.biWidth * infoHeader.biHeight; i++) {
			read(picture, pixel);
			write(newPicture, pixel);
		}

		text.close();
		newPicture.close();
	}
	else {
		cout << "No that command. Terminated..." << endl;
		return -1;
	}

	picture.close();
}