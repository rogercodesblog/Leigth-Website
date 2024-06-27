var embed = new Twitch.Embed("twitch-embed", {
    channel: "leigth",
    autoplay: true
  });

  embed.addEventListener(Twitch.Embed.VIDEO_READY, () => {
    var player = embed.getPlayer();
    player.play();
  });