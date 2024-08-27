namespace HaulThis;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class ConnectionStrings
{
    public string DevelopmentConnection { get; set; }
}