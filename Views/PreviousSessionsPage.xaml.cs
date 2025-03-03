using dotnetMAUI.Flashcards.ViewModels;

namespace dotnetMAUI.Flashcards.Views;

public partial class PreviousSessionsPage : ContentPage
{
	public PreviousSessionsPage(PreviousSessionsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}