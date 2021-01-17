using System;
using System.Globalization;

namespace PhoneStation.PhoneNumber
{
    public class StationUser : IStationUser
    {
        public string Number { get; private set; }
        public string UserName { get; private set; }
        public decimal Money { get; private set; }

        public StationUser(string number, string userName)
        {
            Number = number;
            UserName = userName;
        }

        public StationUser(string number, string userName, decimal money) : this(number, userName)
        {
            Money = money;
        }

        public void ChangeBalance(decimal money)
        {
            Money += money;
        }

        public override string ToString()
        {
            return $"{UserName}, {Number}, ${Math.Round(Money, 2).ToString(CultureInfo.InvariantCulture)}";
        }
    }
}
