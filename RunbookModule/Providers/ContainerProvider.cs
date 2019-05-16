using Ninject;
using RunbookModule.Factories;
using RunbookModule.Report;
using RunbookModule.Validators;
using RunbookModule.Wrappers;
using RunbookModule.Constants;
using RunbookModule.Loggers;

namespace RunbookModule.Providers
{
    public static class ContainerProvider
    {
        private static IKernel _kernel;

        static ContainerProvider()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<IReportCreator>().To<ReportCreator>().InTransientScope();
            _kernel.Bind<ISectionValidator>().To<SectionValidator>().InTransientScope();
            _kernel.Bind<IPsWrapperFactory>().To<PsWrapperFactory>().InSingletonScope();
            _kernel.Bind<IChapterFactory>().To<ChapterFactory>().InSingletonScope();
            _kernel.Bind<IParallelSectionFactroy>().To<ParallelSectionFactroy>().InSingletonScope();
            _kernel.Bind<ISequenceSectionFactory>().To<SequenceSectionFactory>().InSingletonScope();
            _kernel.Bind<IBufferSectionFactory>().To<BufferSectionFactory>().InSingletonScope();
            _kernel.Bind<IWindowSectionFactory>().To<WindowSectionFactory>().InSingletonScope();
            _kernel.Bind<IComposeLoggerFactory>().To<ComposeLoggerFactory>().InTransientScope();
            _kernel.Bind<IFileLoggerFactory>().To<FileLoggerFactory>().InTransientScope();
            _kernel.Bind<IRunbook>().To<Runbook>().InTransientScope();
            _kernel.Bind<ILogger>().To<LiveLogger>().InSingletonScope().Named(ContainerConstants.LiveLogger);
        }

        public static IKernel Container => _kernel;

        public static T Resolve<T>(string name = null)
        {
            return string.IsNullOrEmpty(name) ? Container.Get<T>() : Container.Get<T>(name);
        }
    }
}
