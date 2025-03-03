using dotnetMAUI.Flashcards.ViewModels;

namespace dotnetMAUI.Flashcards.Views;

public partial class StudyPage : ContentPage
{
	public StudyPage(StudyViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}