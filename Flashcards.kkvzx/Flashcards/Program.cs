using Flashcards.Models;
using Flashcards.Presenters;
using Flashcards.Views;

var repository = new MainRepository();
var view = new MainView();
var presenter = new MainPresenter(view, repository);

presenter.Run();