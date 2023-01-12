namespace NTierArchitectureServer.Business.Services.AuthServices.Dtos
{
    public class ResultDto
    {
        public bool IsSucces { get; set; } = true;
        public List<string> Messages { get; set; } = new();
    }
}
