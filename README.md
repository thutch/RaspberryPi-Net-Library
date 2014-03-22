RaspberryPi-Net-Library
=======================

Combining multiple Raspberry Pi libraries to a complete easy to maintain library.

### Example: Turn on the LED on a specific PIN for 1 second and turn it off again

> Required using statements

```c#
using RaspberryPi.GPIO;
using System.Threading;
```

> Code example

```c#
using(GPIOMem led = new GPIOMem(GPIOPins.V2_GPIO_25))
{
  led.Write(true); // Enable the specified PIN
  Thread.Sleep(1000);
  led.Write(false);
  
}
```

### Example PWM usage (only supports RaspberryPi revision 2)

> Required using statements

```c#
using RaspberryPi.GPIO;
using System.Threading;
```

> Code example

```c#
using(GPIOMem pwm = new GPIOMem(GPIOPins.V2_GPIO_PWM, GPIOPwmDivisor.CLK_128, GPIOPwmChannel.Channel_0, GPIOPwmMarkspace.Balanced, 1024))
{
  pwm.SetPWMData(512); // Set the duty cycle
  pwm.PwmEnable(true);
  
  Thread.Sleep(5000); // Let PWM run for 5 seconds and than turn it off (caused by 'using' statement)
}
```

> We do not recommend using GPIOPwmChannel.Channel_1 (it represents the audio jack and does not perform well, output voltage is very low)
