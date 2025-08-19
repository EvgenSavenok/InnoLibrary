namespace Application.Contracts.RequestFeatures;

public abstract record QueryParameters
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    
    public string? OrderBy { get; set; }
    public bool Descending { get; set; } 
}