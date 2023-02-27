using Microsoft.EntityFrameworkCore;

namespace App.Data;

public interface IConfigureModelCreating
{
    void OnModelCreating(ModelBuilder modelBuilder);
}