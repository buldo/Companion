#include <stdio.h>
#include "pico/bootrom.h"
#include "pico/stdlib.h"
#include "hardware/dma.h"
#include "hardware/uart.h"
#include "hardware/gpio.h"
#include "hardware/divider.h"
#include "hardware/spi.h"
#include "hardware/irq.h"
#include "EPD_Test.h"
#include "display.h"
#include "ImageData.h"
#include "tusb.h"

const uint Display_RST_PIN = 12;
const uint Display_DC_PIN = 8;
const uint Display_BUSY_PIN = 13;
const uint Display_CS_PIN = 9;
const uint Display_CLK_PIN = 10;
const uint Display_MOSI_PIN = 11;
spi_inst_t *DisplaySpi = spi1;

void init_pin(uint pin, bool out)
{
    gpio_init(pin);
    gpio_set_dir(pin, out);
}

void InitGpio()
{
    init_pin(Display_RST_PIN, true);
    init_pin(Display_DC_PIN, true);
    init_pin(Display_CS_PIN, true);
    init_pin(Display_BUSY_PIN, false);

    gpio_put(Display_CS_PIN, true);

    spi_init(DisplaySpi, 4000 * 1000);
    gpio_set_function(Display_CLK_PIN, GPIO_FUNC_SPI);
    gpio_set_function(Display_MOSI_PIN, GPIO_FUNC_SPI);
}

const u_int32_t bufferSize = 296 * 128 / 8;

static uint8_t framebuffer[bufferSize];
Drivers::EPaperDisplay *display;

int main(void)
{
    stdio_init_all();
    DEV_Delay_ms(500);
    InitGpio();

    auto d = Drivers::EPaperDisplay(DisplaySpi, Display_RST_PIN, Display_BUSY_PIN, Display_DC_PIN, Display_CS_PIN);
    display = &d;
    display->Init();
    display->Clear();

    for (size_t i = 0; i < bufferSize; i++)
    {
        framebuffer[i] = 0x00;
    }
    // gImage_2in9

    tusb_init();

    while (true)
    {
        tud_task();
        tight_loop_contents();
    }

    return 0;
}

//--------------------------------------------------------------------+
// Device callbacks
//--------------------------------------------------------------------+

// Invoked when device is mounted
void tud_mount_cb(void)
{
}

// Invoked when device is unmounted
void tud_umount_cb(void)
{
}

// Invoked when usb bus is suspended
// remote_wakeup_en : if host allow us  to perform remote wakeup
// Within 7ms, device must draw an average of current less than 2.5 mA from bus
void tud_suspend_cb(bool remote_wakeup_en)
{
    (void)remote_wakeup_en;
}

// Invoked when usb bus is resumed
void tud_resume_cb(void)
{
}

//--------------------------------------------------------------------+
// USB HID
//--------------------------------------------------------------------+

// Invoked when received GET_REPORT control request
// Application must fill buffer report's content and return its length.
// Return zero will cause the stack to STALL request
uint16_t tud_hid_get_report_cb(uint8_t itf, uint8_t report_id, hid_report_type_t report_type, uint8_t *buffer, uint16_t reqlen)
{
    // TODO not Implemented
    (void)itf;
    (void)report_id;
    (void)report_type;
    (void)buffer;
    (void)reqlen;

    return 0;
}

enum Command : uint8_t
{
    PutData = 0x01,
    Display = 0x02,
    RebootToBootloader = 0xFF
};

// Invoked when received SET_REPORT control request or
// received data on OUT endpoint ( Report ID = 0, Type = 0 )
void tud_hid_set_report_cb(uint8_t itf, uint8_t report_id, hid_report_type_t report_type, uint8_t const *buffer, uint16_t bufsize)
{
    auto command = (Command)buffer[0];

    if (command == Command::PutData)
    {
        size_t initOffset = 1;
        uint16_t offset = (uint16_t)buffer[initOffset + 0] | (uint16_t)buffer[initOffset + 1] << 8;

        uint8_t count = buffer[initOffset + 2];
        size_t internalOffset = initOffset + 3;
        for (size_t i = offset; i < count + offset; i++, internalOffset++)
        {
            if (i == 4736)
            {
                framebuffer[i] = buffer[internalOffset];
            }
            framebuffer[i] = buffer[internalOffset];
        }

        return;
    }

    if (command == Command::Display)
    {
        display->DisplayFrameBuffer(framebuffer);
        return;
    }

    if(command == Command::RebootToBootloader)
    {
        reset_usb_boot(0, 0);
        return;
    }
}