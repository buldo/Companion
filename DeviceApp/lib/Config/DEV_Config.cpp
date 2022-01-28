/*****************************************************************************
* | File      	:   DEV_Config.c
* | Author      :   Waveshare team
* | Function    :   Hardware underlying interface
* | Info        :
*----------------
* |	This version:   V3.0
* | Date        :   2019-07-31
* | Info        :   
#
# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documnetation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of theex Software, and to permit persons to  whom the Software is
# furished to do so, subject to the following conditions:
#
# The above copyright notice and this permission notice shall be included in
# all copies or substantial portions of the Software.
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS OR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
# THE SOFTWARE.
#
******************************************************************************/
#include "DEV_Config.h"

#define SPI_PORT spi1

/**
 * GPIO
**/
int EPD_RST_PIN;
int EPD_DC_PIN;
int EPD_CS_PIN;
int EPD_BUSY_PIN;
int EPD_CLK_PIN;
int EPD_MOSI_PIN;

/**
 * SPI
**/
void DEV_SPI_WriteByte(uint8_t Value)
{
    spi_write_blocking(SPI_PORT, &Value, 1);
}

void DEV_SPI_Write_nByte(uint8_t *pData, uint32_t Len)
{
    spi_write_blocking(SPI_PORT, pData, Len);
}

/**
 * delay x ms
**/
void DEV_Delay_ms(UDOUBLE xms)
{
	sleep_ms(xms);
}

void DEV_GPIO_Init(void)
{

	EPD_RST_PIN     = 12;
	EPD_DC_PIN      = 8;
	EPD_BUSY_PIN    = 13;
	
	EPD_CS_PIN      = 9;
	EPD_CLK_PIN		= 10;
	EPD_MOSI_PIN	= 11;
}
/******************************************************************************
function:	Module Initialize, the library and initialize the pins, SPI protocol
parameter:
Info:
******************************************************************************/
UBYTE DEV_Module_Init(void)
{
    // GPIO Config
	DEV_GPIO_Init();
	
    printf("DEV_Module_Init OK \r\n");
	return 0;
}