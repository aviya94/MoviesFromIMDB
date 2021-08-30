Operating Instructions:

Run the program and type in the movie name and click the Search button.
The program will scan the movies that match the search word from the IMDB website.
The program will store this data on each movie: titel, genre,rating,duration,stars,MPAARatingPG
When done, save the movie data in a text file named MyMovie.txt in the folder "c:\temp"
If a file with such a name already exists, the file will be deleted and created.
To clear the search field, click on a button Clear
To exit the program, press a button Exit

clases:

GetDataMovieFromIMDB - A class containing functions that bring the details to each movie.
The class inherits from the abstract class GetDataFromIMDB

GetDataFromIMDB - An abstract class that contains variables and functions that are common to search for movies, series and more

WriteToTextFile - A class that writes into a text file

MovieSearch -Taking the information from the UI screen


