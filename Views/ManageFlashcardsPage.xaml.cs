using dotnetMAUI.Flashcards.ViewModels;

namespace dotnetMAUI.Flashcards.Views;

public partial class ManageFlashcardsPage : ContentPage
{
	public ManageFlashcardsPage(ManageFlashcardsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}