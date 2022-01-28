# Generated Cmake Pico project file

cmake_minimum_required(VERSION 3.13)

#set(CMAKE_C_STANDARD 11)
#set(CMAKE_CXX_STANDARD 17)

# Initialise pico_sdk from installed location
# (note this can come from environment, CMake cache etc)
#set(PICO_SDK_PATH "D:/pico/pico-sdk")

# Pull in Raspberry Pi Pico SDK (must be before project)
include(pico_sdk_import.cmake)

project(Display C CXX ASM)

# Initialise the Raspberry Pi Pico SDK
pico_sdk_init()

# Add executable. Default name is the project name, version 0.1

add_subdirectory(lib/Config)
add_subdirectory(lib/e-Paper)
add_subdirectory(lib/Fonts)
add_subdirectory(lib/GUI)
add_subdirectory(examples)

include_directories(examples)
include_directories(lib/Config)
include_directories(lib/GUI)
include_directories(lib/e-Paper)

add_executable(Display Display.cpp )

target_sources(Display PUBLIC
        ${CMAKE_CURRENT_LIST_DIR}/usb_descriptors.c
        )

target_include_directories(Display PUBLIC
        ${CMAKE_CURRENT_LIST_DIR})

pico_set_program_name(Display "Display")
pico_set_program_version(Display "0.1")

pico_enable_stdio_uart(Display 1)
pico_enable_stdio_usb(Display 0)

pico_add_extra_outputs(Display)

# Add the standard library to the build
#target_link_libraries(Display pico_stdlib ePaper GUI Fonts Config hardware_spi )
target_link_libraries(Display examples ePaper GUI Fonts Config pico_stdlib hardware_spi hardware_dma hardware_irq tinyusb_device tinyusb_board)