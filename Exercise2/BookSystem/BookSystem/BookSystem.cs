using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BookSystem
{
    // Enum for Genre Types
    public enum GenreType
    {
        Fiction,
        NonFiction,
        Fantasy,
        Mystery,
        Biography,
        ScienceFiction,
        Romance
    }

    // Author Class
    public class Author
    {
        // Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactUrl { get; set; }
        public string ResidentCity { get; set; }
        public string ResidentCountry { get; set; }

        // Greedy Constructor
        public Author(string firstName, string lastName, string contactUrl, string residentCity, string residentCountry)
        {
            // Validation for each property
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(contactUrl))
                throw new ArgumentException("Contact URL cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(residentCity))
                throw new ArgumentException("Resident city cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(residentCountry))
                throw new ArgumentException("Resident country cannot be null, empty, or whitespace.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            ContactUrl = ValidateUrl(contactUrl);
            ResidentCity = residentCity.Trim();
            ResidentCountry = residentCountry.Trim();
        }

        // Data Validation for URL using Regex
        private string ValidateUrl(string url)
        {
            string pattern = @"(https?://www\.)?[a-zA-Z0-9]+\.\w{2}(?!\.)";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(url))
            {
                return url;
            }
            throw new ArgumentException("Invalid URL format.");
        }

        // Overriding ToString method
        public override string ToString()
        {
            return $"{FirstName},{LastName},{ContactUrl},{ResidentCity},{ResidentCountry}";
        }
    }

    // Review Class
    public class Review
    {
        // Properties
        public string ISBN { get; set; }
        public Reviewer Reviewer { get; set; }
        public RatingType Rating { get; set; }
        public string Comment { get; set; }

        // Greedy Constructor
        public Review(string isbn, Reviewer reviewer, RatingType rating, string comment)
        {
            // Validation for each property
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN cannot be null, empty, or whitespace.");
            if (reviewer == null)
                throw new ArgumentException("Reviewer cannot be null.");
            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Comment cannot be null, empty, or whitespace.");

            ISBN = isbn.Trim();
            Reviewer = reviewer;
            Rating = rating;
            Comment = comment.Trim();
        }

        // Overriding ToString method
        public override string ToString()
        {
            return $"{ISBN},{Reviewer},{Rating},{Comment}";
        }
    }

    // Reviewer Class
    public class Reviewer
    {
        // Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Organization { get; set; }

        // Greedy Constructor
        public Reviewer(string firstName, string lastName, string email, string organization)
        {
            // Validation for each property
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null, empty, or whitespace.");

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = ValidateEmail(email);

            // Handle the organization assignment
            if (string.IsNullOrWhiteSpace(organization))
            {
                Organization = "Independent"; // Default value when organization is not provided
            }
            else
            {
                Organization = organization.Trim();
            }
        }

        // Email validation method using Regex
        private string ValidateEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern);
            if (regex.IsMatch(email))
            {
                return email;
            }
            throw new ArgumentException("Invalid email format.");
        }

        // Overriding ToString method
        public override string ToString()
        {
            return $"{FirstName},{LastName},{Email},{Organization}";
        }
    }

    // Book Class
    public class Book
    {
        // Properties
        public string ISBN { get; private set; }
        public string Title { get; private set; }
        public int PublishYear { get; private set; }
        public Author Author { get; private set; }
        public GenreType Genre { get; set; }
        public List<Review> Reviews { get; private set; } = new List<Review>();

        // Greedy Constructor
        public Book(string isbn, string title, int publishYear, Author author, GenreType genre)
        {
            // Validation for each property
            if (string.IsNullOrWhiteSpace(isbn))
                throw new ArgumentException("ISBN cannot be null, empty, or whitespace.");
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null, empty, or whitespace.");
            if (publishYear <= 0)
                throw new ArgumentException("Publish year must be a positive, non-zero whole number.");
            if (author == null)
                throw new ArgumentException("Author is required.");

            ISBN = isbn.Trim();
            Title = title.Trim();
            PublishYear = publishYear;
            Author = author;
            Genre = genre;
        }

        // Adding a review
        public void AddReview(Review review)
        {
            if (review == null)
                throw new ArgumentException("Review required.");
            if (review.ISBN != ISBN)
                throw new ArgumentException($"Review ISBN {review.ISBN} does not match Book ISBN {ISBN}.");
            if (Reviews.Exists(r => r.Reviewer.FirstName == review.Reviewer.FirstName && r.Reviewer.LastName == review.Reviewer.LastName))
                throw new ArgumentException($"Reviewer {review.Reviewer.FirstName} {review.Reviewer.LastName} has already submitted a review.");

            Reviews.Add(review);
        }

        // Get total number of reviews
        public int TotalReviews => Reviews.Count;

        // Overriding ToString method
        public override string ToString()
        {
            return $"{ISBN},{Title},{PublishYear},{Author},{Genre},{TotalReviews}";
        }
    }

    // RatingType Enum
    public enum RatingType
    {
        Buy,
        Recommend,
        Avoid
    }
}
