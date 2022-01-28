#include "display.h"
#include "pico/stdlib.h"
#include "time.h"

using namespace Drivers;

EPaperDisplay::EPaperDisplay(
    spi_inst_t *spi,
    uint resetPin,
    uint busyPin,
    uint dcPin,
    uint csPin)
{
    this->_spi = spi;
    this->_resetPin = resetPin;
    this->_busyPin = busyPin;
    this->_dcPin = dcPin;
    this->_csPin = csPin;
}

void EPaperDisplay::DisplayFrameBuffer(uint8_t *image)
{
	this->SendCommand(0x24);   //write RAM for black(0)/white (1)
	for(int i=0;i<4736;i++)
	{
		this->SendData(image[i]);
	}
	this->TurnOnDisplay();	
}

void EPaperDisplay::TurnOnDisplay()
{
	this->SendCommand(0x22); //Display Update Control
	this->SendData(0xF7);
	this->SendCommand(0x20); //Activate Display Update Sequence
	this->ReadBusy();
}

void EPaperDisplay::Clear()
{
	this->SendCommand(0x24);   //write RAM for black(0)/white (1)
	for(int i=0;i<4736;i++)
	{
		this->SendData(0xff);
	}
	this->TurnOnDisplay();
}

void EPaperDisplay::ReadBusy()
{
    while(1)
	{	 //=1 BUSY
		if(gpio_get(this->_busyPin)==false) 
			break;
		sleep_ms(50);
	}
	sleep_ms(50);
}

void EPaperDisplay::Init()
{
    this->Reset();
	sleep_ms(100);

	this->ReadBusy();
	this->SendCommand(0x12); // soft reset
	this->ReadBusy();
	
	this->SendCommand(0x01); //Driver output control      
	this->SendData(0x27);
	this->SendData(0x01);
	this->SendData(0x00);
	
	this->SendCommand(0x11); //data entry mode       
	this->SendData(0x03);
	
	this->SetWindows(0, 0, this->WIDTH-1, this->HEIGHT-1);
	
	this->SendCommand(0x21); //  Display update control
	this->SendData(0x00);
	this->SendData(0x80);	
	
	this->SetCursor(0, 0);
	this->ReadBusy();	
}

void EPaperDisplay::SendCommand(uint8_t command)
{
    gpio_put(this->_dcPin, false);
    gpio_put(this->_csPin, false);
    spi_write_blocking(this->_spi, &command, 1);
    gpio_put(this->_csPin, true);
}

void EPaperDisplay::SendData(uint8_t data)
{
    gpio_put(this->_dcPin, true);
    gpio_put(this->_csPin, false);
    spi_write_blocking(this->_spi, &data, 1);
    gpio_put(this->_csPin, true);
}

void EPaperDisplay::SetWindows(uint16_t xStart, uint16_t yStart, uint16_t xEnd, uint16_t yEnd)
{
    this->SendCommand(0x44); // SET_RAM_X_ADDRESS_START_END_POSITION
    this->SendData((xStart>>3) & 0xFF);
    this->SendData((xEnd>>3) & 0xFF);
	
    this->SendCommand(0x45); // SET_RAM_Y_ADDRESS_START_END_POSITION
    this->SendData(yStart & 0xFF);
    this->SendData((yStart >> 8) & 0xFF);
    this->SendData(yEnd & 0xFF);
    this->SendData((yEnd >> 8) & 0xFF);
}

void EPaperDisplay::SetCursor(uint16_t xStart, uint16_t yStart)
{
    this->SendCommand(0x4E); // SET_RAM_X_ADDRESS_COUNTER
    this->SendData(xStart & 0xFF);

    this->SendCommand(0x4F); // SET_RAM_Y_ADDRESS_COUNTER
    this->SendData(yStart & 0xFF);
    this->SendData((yStart >> 8) & 0xFF);
}

void EPaperDisplay::Reset()
{
    gpio_put(_resetPin, true);
    sleep_ms(100);
    gpio_put(_resetPin, false);
    sleep_ms(1);
    gpio_put(_resetPin, true);
    sleep_ms(100);
}