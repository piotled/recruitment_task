﻿namespace RecruitmentTask.Api.DataAccess;

public class Subcategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public int CategoryId { get; set; }
}
