﻿namespace IssueProvider.Application.Interfaces;

public interface IModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string State { get; set; }
}