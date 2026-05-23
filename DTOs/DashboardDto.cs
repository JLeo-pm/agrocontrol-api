namespace SmartRancho.API.Dtos;

public class DashboardDto
{
    public int TotalAnimales { get; set; }
    public int Activos { get; set; }
    public int Vendidos { get; set; }
    public int Muertos { get; set; }
    public int Perdidos { get; set; }
    public int TotalPotreros { get; set; }

    public List<AnimalesPorPotreroDto> AnimalesPorPotrero { get; set; }
}

public class AnimalesPorPotreroDto
{
    public string Potrero { get; set; }
    public int Cantidad { get; set; }
}