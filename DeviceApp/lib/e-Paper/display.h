#pragma once
#include "hardware/spi.h"

namespace Drivers
{
    class EPaperDisplay
    {
        private:
            const uint16_t WIDTH = 128;
            const uint16_t HEIGHT = 296;
            spi_inst_t* _spi;
            uint _resetPin;
            uint _busyPin;
            uint _dcPin;
            uint _csPin;
            void Reset();
            void ReadBusy();
            void SendCommand(uint8_t command);
            void SendData(uint8_t data);
            void SetWindows(uint16_t xStart, uint16_t yStart, uint16_t xEnd, uint16_t yEnd);
            void SetCursor(uint16_t xStart, uint16_t yStart);
            void TurnOnDisplay();

        public:
            EPaperDisplay(
                spi_inst_t *spi,
                uint resetPin,
                uint busyPin,
                uint dcPin,
                uint csPin);
            void Init();
            void Clear();
            void DisplayFrameBuffer(uint8_t *image);
    };
}