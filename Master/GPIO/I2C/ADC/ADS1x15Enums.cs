using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.GPIO.I2C.ADC
{
    // ADS1x15 Model selection
    public enum ADSIdentifier : byte
    {
        ADS1015 = 0x00,
        ADS1115 = 0x01
    }

    // Pointer Registers
    public enum ADSPointerRegister : byte
    {
        ADS1015_MASK = 0x03,
        ADS1015_CONVERT = 0x00,
        ADS1015_CONFIG = 0x01,
        ADS1015_LOWTHRESH = 0x02,
        ADS1015_HITHRESH = 0x03
    }

    // Config registers
    public enum ADSConfigRegister : int
    {
        ADS1015_OS_MASK = 0x8000,
        ADS1015_OS_SINGLE = 0x8000,  // Write: Set to start a single-conversion
        ADS1015_OS_BUSY = 0x0000,  // Read: Bit = 0 when conversion is in progress
        ADS1015_OS_NOTBUSY = 0x8000,  // Read: Bit = 1 when device is not performing a conversion

        ADS1015_MUX_MASK = 0x7000,
        ADS1015_MUX_DIFF_0_1 = 0x0000,  // Differential P = AIN0, N = AIN1 (default)
        ADS1015_MUX_DIFF_0_3 = 0x1000,  // Differential P = AIN0, N = AIN3
        ADS1015_MUX_DIFF_1_3 = 0x2000,  // Differential P = AIN1, N = AIN3
        ADS1015_MUX_DIFF_2_3 = 0x3000,  // Differential P = AIN2, N = AIN3
        ADS1015_MUX_SINGLE_0 = 0x4000,  // Single-ended AIN0
        ADS1015_MUX_SINGLE_1 = 0x5000,  // Single-ended AIN1
        ADS1015_MUX_SINGLE_2 = 0x6000,  // Single-ended AIN2
        ADS1015_MUX_SINGLE_3 = 0x7000,  // Single-ended AIN3

        ADS1015_PGA_MASK = 0x0E00,
        ADS1015_PGA_6_144V = 0x0000,  // +/-6.144V range
        ADS1015_PGA_4_096V = 0x0200,  // +/-4.096V range
        ADS1015_PGA_2_048V = 0x0400,  // +/-2.048V range (default)
        ADS1015_PGA_1_024V = 0x0600,  // +/-1.024V range
        ADS1015_PGA_0_512V = 0x0800,  // +/-0.512V range
        ADS1015_PGA_0_256V = 0x0A00,  // +/-0.256V range

        ADS1015_MODE_MASK = 0x0100,
        ADS1015_MODE_CONTIN = 0x0000,  // Continuous conversion mode
        ADS1015_MODE_SINGLE = 0x0100,  // Power-down single-shot mode (default)

        ADS1015_DR_MASK = 0x00E0,
        ADS1015_DR_128SPS = 0x0000,  // 128 samples per second
        ADS1015_DR_250SPS = 0x0020,  // 250 samples per second
        ADS1015_DR_490SPS = 0x0040,  // 490 samples per second
        ADS1015_DR_920SPS = 0x0060,  // 920 samples per second
        ADS1015_DR_1600SPS = 0x0080,  // 1600 samples per second (default)
        ADS1015_DR_2400SPS = 0x00A0,  // 2400 samples per second
        ADS1015_DR_3300SPS = 0x00C0,  // 3300 samples per second (also 0x00E0)

        ADS1115_DR_8SPS = 0x0000,  // 8 samples per second
        ADS1115_DR_16SPS = 0x0020,  // 16 samples per second
        ADS1115_DR_32SPS = 0x0040,  // 32 samples per second
        ADS1115_DR_64SPS = 0x0060,  // 64 samples per second
        ADS1115_DR_128SPS = 0x0080,  // 128 samples per second
        ADS1115_DR_250SPS = 0x00A0,  // 250 samples per second (default)
        ADS1115_DR_475SPS = 0x00C0,  // 475 samples per second
        ADS1115_DR_860SPS = 0x00E0,  // 860 samples per second

        ADS1015_CMODE_MASK = 0x0010,
        ADS1015_CMODE_TRAD = 0x0000,  // Traditional comparator with hysteresis (default)
        ADS1015_CMODE_WINDOW = 0x0010,  // Window comparator

        ADS1015_CPOL_MASK = 0x0008,
        ADS1015_CPOL_ACTVLOW = 0x0000,  // ALERT/RDY pin is low when active (default)
        ADS1015_CPOL_ACTVHI = 0x0008,  // ALERT/RDY pin is high when active

        ADS1015_CLAT_MASK = 0x0004,  // Determines if ALERT/RDY pin latches once asserted
        ADS1015_CLAT_NONLAT = 0x0000,  // Non-latching comparator (default)
        ADS1015_CLAT_LATCH = 0x0004,  // Latching comparator

        ADS1015_CQUE_MASK = 0x0003,
        ADS1015_CQUE_1CONV = 0x0000,  // Assert ALERT/RDY after one conversions
        ADS1015_CQUE_2CONV = 0x0001,  // Assert ALERT/RDY after two conversions
        ADS1015_CQUE_4CONV = 0x0002,  // Assert ALERT/RDY after four conversions
        ADS1015_CQUE_NONE = 0x0003,  // Disable the comparator and put ALERT/RDY in high state (default)
    }

    public enum ADS1115SPS  // Samples per seconds for ADS1115
    {
        SPS8 = ADSConfigRegister.ADS1115_DR_8SPS,
        SPS16 = ADSConfigRegister.ADS1115_DR_16SPS,
        SPS32 = ADSConfigRegister.ADS1115_DR_32SPS,
        SPS64 = ADSConfigRegister.ADS1115_DR_64SPS,
        SPS128 = ADSConfigRegister.ADS1115_DR_128SPS,
        SPS250 = ADSConfigRegister.ADS1115_DR_250SPS,
        SPS475 = ADSConfigRegister.ADS1115_DR_475SPS,
        SPS860 = ADSConfigRegister.ADS1115_DR_860SPS
    }
    public enum ADS1015SPS   // Samples per seconds for ADS1015
    {
        SPS128 = ADSConfigRegister.ADS1015_DR_128SPS,
        SPS250 = ADSConfigRegister.ADS1015_DR_250SPS,
        SPS490 = ADSConfigRegister.ADS1015_DR_490SPS,
        SPS920 = ADSConfigRegister.ADS1015_DR_920SPS,
        SPS1600 = ADSConfigRegister.ADS1015_DR_1600SPS,
        SPS2400 = ADSConfigRegister.ADS1015_DR_2400SPS,
        SPS3300 = ADSConfigRegister.ADS1015_DR_3300SPS,
    }

    public enum ADSPGA1x15     // Programmable gain amplifier setting
    {
        PGA6144 = ADSConfigRegister.ADS1015_PGA_6_144V,
        PGA4096 = ADSConfigRegister.ADS1015_PGA_4_096V,
        PGA2048 = ADSConfigRegister.ADS1015_PGA_2_048V,
        PGA1024 = ADSConfigRegister.ADS1015_PGA_1_024V,
        PGA512 = ADSConfigRegister.ADS1015_PGA_0_512V,
        PGA256 = ADSConfigRegister.ADS1015_PGA_0_256V
    }

    // ADC Channel selection
    public enum ADSChannel : int
    {
        Channel0 = 0,
        Channel1 = 1,
        Channel2 = 2,
        Channel3 = 3
    }
}
