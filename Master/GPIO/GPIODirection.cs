using System;
using System.Collections.Generic;
using System.Text;

namespace RaspberryPi.GPIO
{
    /// <summary>
    /// Specifies the direction of the GPIO port
    /// </summary>
    public enum GPIODirection : uint
    {
        In = 0,
        Out = 1,
        Pwm = 2

        //For future use
        //BCM2835_GPIO_FSEL_INPT = 0b000, 
        //BCM2835_GPIO_FSEL_OUTP = 0b001, 
        //BCM2835_GPIO_FSEL_ALT0 = 0b100, 
        //BCM2835_GPIO_FSEL_ALT1 = 0b101,
        //BCM2835_GPIO_FSEL_ALT2 = 0b110, 
        //BCM2835_GPIO_FSEL_ALT3 = 0b111, 
        //BCM2835_GPIO_FSEL_ALT4 = 0b011, 
        //BCM2835_GPIO_FSEL_ALT5 = 0b010,
        //BCM2835_GPIO_FSEL_MASK = 0b111

    };
}
