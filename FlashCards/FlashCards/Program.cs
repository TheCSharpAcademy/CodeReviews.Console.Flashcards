using Microsoft.Extensions.DependencyInjection;

namespace FlashCards
{
    internal class Program
    {
        static void Main(string[] args)
        {

            ServiceCollection services = new ServiceCollection();

            SetServices(services);

            var serviceProvider = services.BuildServiceProvider();

            var app = serviceProvider.GetRequiredService<FlashCardApp>();
            app.Run();


            /*ICardStackRepository cardStackRepository = new CardStackRepository(connectionString);
            IFlashCardRepository flashCardRepository = new FlashCardRepository(connectionString);
            IStudySessionRepository studySessionRepository = new StudySessionRepository(connectionString);

            IFlashCardAppUi appUi = new FlashCardAppUi();
            ICardStackServiceUi cardStackUi = new CardStackServiceUi();
            IFlashCardServiceUi flashCardServiceUi = new FlashCardServiceUi();
            IStudySessionServiceUi studySessionUi = new StudySessionServiceUi();

            ICardStackService cardStackService = new CardStackService(cardStackRepository, cardStackUi);
            IFlashCardService flashCardService = new FlashCardService(flashCardRepository, flashCardServiceUi);
            IStudySessionService studySessionService = new StudySessionService(studySessionRepository, studySessionUi);


           

            FlashCardApp app = new FlashCardApp(cardStackService, flashCardService, studySessionService, appUi, pathToDefaultData, true);
            app.Run();*/

        }
        static void SetServices(IServiceCollection services)
        {
            string connectionString = @"Data Source=(localdb)\LOCALDB;Initial Catalog=FlashCardsProject;Integrated Security=True;Connect Timeout=5;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
            string pathToDefaultData = @"DefaultDataForAutoFill.json";
            //Data Layer
            services.AddSingleton<ICardStackRepository>(new CardStackRepository(connectionString));
            services.AddSingleton<IFlashCardRepository>(new FlashCardRepository(connectionString));
            services.AddSingleton<IStudySessionRepository>(new StudySessionRepository(connectionString));

            // UI Services
            services.AddSingleton<IFlashCardAppUi, FlashCardAppUi>();
            services.AddSingleton<ICardStackServiceUi, CardStackServiceUi>();
            services.AddSingleton<IFlashCardServiceUi, FlashCardServiceUi>();
            services.AddSingleton<IStudySessionServiceUi, StudySessionServiceUi>();

            // Business Logic Services
            services.AddSingleton<ICardStackService, CardStackService>();
            services.AddSingleton<IFlashCardService, FlashCardService>();
            services.AddSingleton<IStudySessionService, StudySessionService>();

            services.AddSingleton(sp => new FlashCardApp(
                sp.GetRequiredService<ICardStackService>(),
                sp.GetRequiredService<IFlashCardService>(),
                sp.GetRequiredService<IStudySessionService>(),
                sp.GetRequiredService<IFlashCardAppUi>(),
                pathToDefaultData,
                autoFill: true
            ));
        }
    }

}
