﻿namespace SilentMike.DietMenu.Shared.MassTransit;

public interface ICreatedFamilyMessage
{
    Guid Id { get; }
    string Name { get; }
}