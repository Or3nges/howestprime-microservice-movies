/*
using Howestprime.Movies.Domain.Facilities;
using Howestprime.Movies.Domain.Movies;
*/
using Microsoft.EntityFrameworkCore;
using Howestprime.Movies.Domain.Entities;
using Domaincrafters.Domain;

namespace Howestprime.Movies.Infrastructure.Persistence.EntityFramework.Configurations;
/**
    * Seeder class to seed the database with initial data.
    * Replace the commented code with actual data seeding logic.
*/

public static class Seeder
{
    /*
    private static readonly IList<Movie> _movies = [
        Movie.Create(new MovieId("ebfb9308-6c61-4608-af77-394448808e9b"),
        "The Matrix",
        "A computer hacker learns from mysterious rebels about the true nature of his reality",
        1999,
        136,
        "Sci-fi",
        "Keanu Reeves, Laurence Fishburne, Carrie-Anne Moss",
        16,
        "https://www.imdb.com/title/tt0133093/"),

        Movie.Create(new MovieId("fb258d1a-10a2-4bf9-85cd-ca83585d1ee5"),
        "The Matrix Reloaded",
        "The human city of Zion defends itself against the massive invasion of the machines as Neo fights to end the war at another front while also opposing the rogue Agent Smith.",
        2003,
        138,
        "Sci-fi",
        "Keanu Reeves, Laurence Fishburne, Carrie-Anne Moss",
        16,
        "https://www.imdb.com/title/tt0234215/")
    ];
    
    private static readonly IList<Room> _rooms = [
        Room.Create("Room 1", 100, new("f38145ab-9f1e-4778-90f4-b911fb5e15a7")),
        Room.Create("Room 2", 200, new("45bfe58a-c9ba-44b6-911e-c1387f6e1ace"))
    ];
    */

    private static readonly IList<Room> _rooms = [
        new Room(
            new RoomId("1a38a0f1-6539-4c71-a938-47636088783b"),
            "Red Room",
            200
        ),
        new Room(
            new RoomId("1208ee1a-2cfb-4159-b311-8bf842861bc6"),
            "Blue Room",
            100
        )
    ];

    public static void Seed(this ModelBuilder modelBuilder)
    {
        /*
        modelBuilder.Entity<Room>().HasData(_rooms);
        modelBuilder.Entity<Movie>().HasData(_movies);
        */
        modelBuilder.Entity<Room>().HasData(_rooms);
    }
}