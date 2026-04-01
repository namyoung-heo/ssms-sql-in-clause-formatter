using System;
using System.ComponentModel.Design;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace SqlInClauseFormatter
{
    /// <summary>
    /// 선택된 텍스트를 SQL IN절로 변환하는 커맨드 핸들러
    /// </summary>
    internal sealed class ConvertToInClauseCommand
    {
        /// <summary>
        /// .vsct에 정의된 Command ID
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// .vsct에 정의된 Command Set GUID
        /// </summary>
        public static readonly Guid CommandSet = new Guid("c2d3e4f5-a6b7-4890-1bcd-ef1234567890");

        private readonly AsyncPackage package;

        private ConvertToInClauseCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        public static ConvertToInClauseCommand Instance { get; private set; }

        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider => this.package;

        /// <summary>
        /// 패키지 초기화 시 호출 - 커맨드 인스턴스 생성
        /// </summary>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ConvertToInClauseCommand(package, commandService);
        }

        /// <summary>
        /// 커맨드 실행 - 선택 텍스트를 IN절로 변환하여 교체
        /// </summary>
        private void Execute(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // DTE (Development Tools Environment) 가져오기
            DTE2 dte = Package.GetGlobalService(typeof(DTE)) as DTE2;
            if (dte == null)
                return;

            // 활성 문서의 텍스트 선택 영역 가져오기
            TextSelection selection = dte.ActiveDocument?.Selection as TextSelection;
            if (selection == null || string.IsNullOrEmpty(selection.Text))
            {
                VsShellUtilities.ShowMessageBox(
                    this.package,
                    "변환할 텍스트를 먼저 선택해주세요.",
                    "SQL IN절 변환",
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
                return;
            }

            // 선택된 텍스트 변환
            string originalText = selection.Text;
            string convertedText = TextTransformer.ConvertToInClause(originalText);

            // 선택 영역을 변환 결과로 교체
            selection.Insert(convertedText, (int)vsInsertFlags.vsInsertFlagsContainNewText);
        }
    }
}
