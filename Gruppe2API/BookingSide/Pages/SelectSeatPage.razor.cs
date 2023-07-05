using Microsoft.AspNetCore.Components;
using Models;

namespace BookingSide.Pages
{
    public partial class SelectSeatPage
    {
        [Parameter]
        public Stadium SelectedStadium { get; set; }
        [Parameter]
        public MatchInfo SelectedMatch { get; set; }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
           if(firstRender) {
            
           }
        }





    }
}
