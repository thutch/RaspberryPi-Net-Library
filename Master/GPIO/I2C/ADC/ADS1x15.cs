using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspberryPi.GPIO.I2C.ADC
{
    public class ADS1x15
    {
        private bool debug;

        private bool continuousRead = false;

        private int address;
        private string devBus;

        private ADSIdentifier model;
        private I2CBus bus;


        public ADS1x15(byte address, ADSIdentifier model)
        {
            HandlePiVersion();
            this.address = address;
            this.model = model;
        }

        // Start Continuous Read Mode
        private void StartContinuousReadADC(ADSChannel channel, ADSPGA1x15 pga, int sps)
        {

            // Disable comparator, Non-latching, Alert/Rdy active low
            // traditional comparator, single-shot mode

            int config = (int)ADSConfigRegister.ADS1015_CQUE_NONE |
                         (int)ADSConfigRegister.ADS1015_CLAT_NONLAT |
                         (int)ADSConfigRegister.ADS1015_CPOL_ACTVLOW |
                         (int)ADSConfigRegister.ADS1015_CMODE_TRAD |
                         (int)ADSConfigRegister.ADS1015_MODE_CONTIN;

            config |= (int)sps; // Add samples per seconds bits to config
            config |= (int)pga; // Add programmable gain amplifier bits to config

            switch ((int)channel)
            {
                case 0:
                    config |= (int)ADSConfigRegister.ADS1015_MUX_SINGLE_0;
                    break;
                case 1:
                    config |= (int)ADSConfigRegister.ADS1015_MUX_SINGLE_1;
                    break;
                case 2:
                    config |= (int)ADSConfigRegister.ADS1015_MUX_SINGLE_2;
                    break;
                case 3:
                    config |= (int)ADSConfigRegister.ADS1015_MUX_SINGLE_3;
                    break;
                default:
                    break;
            }

            // set start single-conversion bit
            config |= (int)ADSConfigRegister.ADS1015_OS_SINGLE;

            // write config register to ADC
            byte[] bytes = { (byte)((((byte)(config >> 8))) & 0xFF), (byte)(((byte)config) & 0xFF) };
            WriteCommand((byte)ADSPointerRegister.ADS1015_CONFIG, bytes);

            // Sleep for chip to proccess
            System.Threading.Thread.Sleep(10);

            //set continuousRead to true
            this.continuousRead = true;

        }

        // Read when using Continuous Mode
        public double ReadLastContinuousRead()
        {
            // handle exception if no start method was called.
            if (this.continuousRead != true)
                throw new ADS1x15Exception("Cannot retrieve last read, startContinuousReadADC expected.");

            // set convertedResult to error
            double convertedResult = -1;

            // reset config register to convert
            WritePointer((byte)ADSPointerRegister.ADS1015_CONVERT);

            // read result
            byte[] readResult = ReadBytes();

            //convert result from byte to double
            if (model == ADSIdentifier.ADS1015)
            {
                convertedResult = (((readResult[0] << 8) | (readResult[1] & 0xFF)) >> 4) * 6144 / 2048.0;
            }
            else if (model == ADSIdentifier.ADS1115)
            {
                convertedResult = (readResult[0] << 8) | (readResult[1]);
            }
            return convertedResult;
        }

        // Stop Continuous Mode
        public bool StopContinuousReadADC()
        {
            int config = 0x8583; // Refer to ADS1x15 Datasheet

            // write config register to ADC
            byte[] bytes = { (byte)((((byte)(config >> 8))) & 0xFF), (byte)(((byte)config) & 0xFF) };
            WriteCommand((byte)ADSPointerRegister.ADS1015_CONFIG, bytes);

            // set continuousRead to false
            this.continuousRead = false;

            return true;
        }

        // Read ADC Single Converstion (CHIP returns value then enters sleep mode)
        private double ReadADCSingleEnded(ADSChannel channel, ADSPGA1x15 pga, int sps)
        {
            //System.Threading.Thread.Sleep(2);

            double convertedResult = -1;
            // Disable comparator, Non-latching, Alert/Rdy active low
            // traditional comparator, single-shot mode

            int config = (int)ADSConfigRegister.ADS1015_CQUE_NONE |
                         (int)ADSConfigRegister.ADS1015_CLAT_NONLAT |
                         (int)ADSConfigRegister.ADS1015_CPOL_ACTVLOW |
                         (int)ADSConfigRegister.ADS1015_CMODE_TRAD |
                         (int)ADSConfigRegister.ADS1015_MODE_SINGLE;

            config |= (int)sps; // Add samples per seconds bits to config
            config |= (int)pga; // Add programmable gain amplifier bits to config

            switch ((int)channel)
            {
                case 0:
                    config |= (int)ADSConfigRegister.ADS1015_MUX_SINGLE_0;
                    break;
                case 1:
                    config |= (int)ADSConfigRegister.ADS1015_MUX_SINGLE_1;
                    break;
                case 2:
                    config |= (int)ADSConfigRegister.ADS1015_MUX_SINGLE_2;
                    break;
                case 3:
                    config |= (int)ADSConfigRegister.ADS1015_MUX_SINGLE_3;
                    break;
                default:
                    break;
            }

            //set start single-conversion bit
            config |= (int)ADSConfigRegister.ADS1015_OS_SINGLE;

            //write config register to ADC
            byte[] bytes = { (byte)((((byte)(config >> 8))) & 0xFF), (byte)(((byte)config) & 0xFF) };
            WriteCommand((byte)ADSPointerRegister.ADS1015_CONFIG, bytes);

            //Sleep for chip to proccess
            System.Threading.Thread.Sleep(10);

            //reset config register to convert
            WritePointer((byte)ADSPointerRegister.ADS1015_CONVERT);

            //read result
            byte[] readResult = ReadBytes();

            //convert result from byte to double
            if (model == ADSIdentifier.ADS1015)
            {
                convertedResult = (((readResult[0] << 8) | (readResult[1] & 0xFF)) >> 4) * 6144 / 2048.0;
            }
            else if (model == ADSIdentifier.ADS1115)
            {
                convertedResult = (readResult[0] << 8) | (readResult[1]);
            }
            return convertedResult;
        }

        #region preparation methods

        public void StartContinuousReadADC(ADSChannel channel, ADSPGA1x15 pga, ADS1115SPS sps)
        {
            //FOR ADS1115
            if (model.Equals(ADSIdentifier.ADS1115))
                StartContinuousReadADC(channel, pga, (int)sps);
            else
                throw new ADS1x15Exception("Invalid sample type object. ADS1015 expected.");
        }

        //Samples arguments for ADS1015
        public void StartContinuousReadADC(ADSChannel channel, ADSPGA1x15 pga, ADS1015SPS sps)
        {
            //FOR ADS1015
            if (model.Equals(ADSIdentifier.ADS1015))
                StartContinuousReadADC(channel, pga, (int)sps);
            else
                throw new ADS1x15Exception("Invalid sample type object. ADS1115 expected.");
        }

        //Samples arguments for ADS1115
        public double ReadADCSingleEnded(ADSChannel channel, ADSPGA1x15 pga, ADS1115SPS sps)
        {
            //FOR ADS1115
            if (model.Equals(ADSIdentifier.ADS1115))
                return ReadADCSingleEnded(channel, pga, (int)sps);
            else
                throw new ADS1x15Exception("Invalid sample type object. ADS1015 expected.");
        }

        //Samples arguments for ADS1015
        public double ReadADCSingleEnded(ADSChannel channel, ADSPGA1x15 pga, ADS1015SPS sps)
        {
            //FOR ADS1015
            if (model.Equals(ADSIdentifier.ADS1015))
                return ReadADCSingleEnded(channel, pga, (int)sps);
            else
                throw new ADS1x15Exception("Invalid sample type object. ADS1115 expected.");
        }

        //Get PI version to auto select I2C bus
        private void HandlePiVersion()
        {
            int rpiRev = RaspberryPi.Common.Revision.Get();

            if (rpiRev == 0)
                throw new RaspberryPi.Common.RevisionException("Raspberry PI revision Beta not supported.");
            else if (rpiRev.Equals(1))
                devBus = I2CBus.DevR1;
            else if (rpiRev.Equals(2))
                devBus = I2CBus.DevR2;
        }

        #endregion

        private void WriteCommand(byte pointer, byte[] value)
        {
            if (this.debug)
                DebugWriteCommand(pointer, value);

            bus.WriteCommand(this.address, pointer, value);
        }

        private void WritePointer(byte pointer)
        {
            if (this.debug)
                DebugWritePointer(pointer);

            bus.WriteByte(this.address, pointer);
        }

        private byte[] ReadBytes()
        {
            byte[] result = bus.ReadBytes(this.address, 2);

            if (this.debug)
                DebugReadBytes(result);

            return result;
        }

        public void Open()
        {
            this.bus = I2CBus.Open(devBus);
        }

        public void Close()
        {
            this.bus.Finalyze();
            this.bus.Dispose();
        }

        #region Property methods

        public bool ContinuousModeEnabled
        {
            get { return this.continuousRead; }
        }

        public bool Debug //property for set Debug
        {
            get { return debug; }
            set { this.debug = value; }
        }

        public bool DriverDebug
        {
            set { this.bus.Debug(value); }
        }

        #endregion

        #region DebugMethods
        private void DebugWriteCommand(byte pointer, byte[] value)
        {
            Console.WriteLine();
            Console.WriteLine("Debug: Writing To I2C Device");
            Console.WriteLine("Debug: address: {0}, pointer: {1}", this.address, pointer);
            Console.Write("Debug: byte array: [ ");
            foreach (byte b in value)
            {
                Console.Write(b + ",");
            }
            Console.Write(" ]");
            Console.WriteLine();
        }

        private void DebugWritePointer(byte pointer)
        {
            Console.WriteLine();
            Console.WriteLine("Debug: Writing Pointer To I2C Device");
            Console.WriteLine("Debug: address: {0}, pointer: {1}", this.address, pointer);
            Console.WriteLine();
        }

        private void DebugReadBytes(byte[] result)
        {
            Console.WriteLine("Debug: Reading From I2C Device");
            Console.WriteLine("Debug: address: {0}", this.address);
            Console.Write("Debug: byte array: [ ");
            foreach (byte b in result)
            {
                Console.Write(b + ",");
            }
            Console.Write(" ]");
            Console.WriteLine();
            Console.WriteLine();
        }
        #endregion
    }
}
