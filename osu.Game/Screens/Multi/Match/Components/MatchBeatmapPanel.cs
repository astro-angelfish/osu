﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Online.API;
using osu.Game.Online.API.Requests;
using osu.Game.Overlays.Direct;
using osu.Game.Rulesets;

namespace osu.Game.Screens.Multi.Match.Components
{
    public class MatchBeatmapPanel : MultiplayerComposite
    {
        [Resolved]
        private IAPIProvider api { get; set; }

        [Resolved]
        private RulesetStore rulesets { get; set; }

        private GetBeatmapSetRequest request;

        public MatchBeatmapPanel()
        {
            AutoSizeAxes = Axes.Both;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            CurrentItem.BindValueChanged(item =>
            {
                var onlineId = item.NewValue?.Beatmap.OnlineBeatmapID ?? 0;

                if (onlineId != 0)
                {
                    request?.Cancel();
                    request = new GetBeatmapSetRequest(onlineId, BeatmapSetLookupType.BeatmapId);
                    request.Success += beatmap =>
                    {
                        ClearInternal();
                        var panel = new DirectGridPanel(beatmap.ToBeatmapSet(rulesets));
                        LoadComponentAsync(panel, p => { AddInternal(panel); });
                    };
                    api.Queue(request);
                }
            }, true);
        }
    }
}
