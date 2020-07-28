using System;

namespace tasktTesting
{
    /// <summary>
    /// Contains a bunch of samples for testing DLL functionality
    /// </summary>
    public class RobotWorker
    {
        /// <summary>
        /// returning a string
        /// </summary>
        /// <returns></returns>
        public string SaySomething()
        {
            return "Hello World!";
        }
        /// <summary>
        /// returning an integer
        /// </summary>
        /// <returns></returns>
        public int FilesProcessed()
        {
            return 5;
        }
        /// <summary>
        /// returning a double
        /// </summary>
        /// <returns></returns>
        public double EstimatedTimeRemaining()
        {
            return 3000024.938272;
        }
        /// <summary>
        /// returning a decimal
        /// </summary>
        /// <returns></returns>
        public decimal AccountBalance()
        {
            return 9383838383 * 303002982;
        }

        /// <summary>
        /// returns a calculation, required input parameter
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public decimal DoMath(int number)
        {
            return number * 2;
        }

        public ComplexModel ReturnComplexItem()
        {
            return new ComplexModel() { SomeName = "This is a complex object!", SomeStatus = 200 };
        }

        public bool ReturnOpposite(bool boolean)
        {
            return !boolean;
        }
        public string ReturnAppendedString(string userValue, string anotherValue)
        {
            return userValue + anotherValue;
        }
    }

    public class ComplexModel
    {
        public string SomeName { get; set; }
        public int SomeStatus { get; set; }
    }

}
