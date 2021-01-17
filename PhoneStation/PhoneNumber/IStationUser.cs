namespace PhoneStation.PhoneNumber
{
    public interface IStationUser
    {
        string Number { get; }
        string UserName { get; }
        decimal Money { get; }
        void ChangeBalance(decimal money);
    }
}
