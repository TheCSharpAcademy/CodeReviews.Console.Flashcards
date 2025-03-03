using dotnetMAUI.Flashcards.ViewModels;

namespace dotnetMAUI.Flashcards.Views;

public partial class ManageStacksPage : ContentPage
{
	public ManageStacksPage(ManageStacksViewModel viewModel) 
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
}