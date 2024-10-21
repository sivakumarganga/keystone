namespace KeyStone.Shared.API.RequestModels
{
    public record SignUpRequest
        (string UserName, string Name, string FamilyName, string Password, string PhoneNumber);
}
