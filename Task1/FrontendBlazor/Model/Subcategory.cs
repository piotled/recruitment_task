﻿namespace FrontendBlazor.Model;

public class Subcategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CategoryId { get; set; }
}
