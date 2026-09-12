using System;
using BookDatabaseApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BookDatabaseApp.Services;

public class BookContext : DbContext
{
    public DbSet<Book> Books { get; set; }
    
    public string DbPath { get; }

    public BookContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "book.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) 
        => options.UseSqlite($"Data Source={DbPath}");
}