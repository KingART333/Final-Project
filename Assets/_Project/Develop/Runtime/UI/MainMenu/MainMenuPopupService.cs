using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.MainMenu
{
    public class MainMenuPopupService : PopupService
    {
        private readonly MainMenuUIRoot _uiRoot;

        public MainMenuPopupService(ViewsFactory viewsFactory, ProjectPresentersFactory presentersFactory, MainMenuUIRoot uIRoot) : base(viewsFactory, presentersFactory)
        {
            _uiRoot = uIRoot;
        }

        protected override Transform PopupLayer => _uiRoot.PopupsLayer;
    }
}
