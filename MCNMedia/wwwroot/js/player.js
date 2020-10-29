jwplayer.key = "SNzmJfLzOyLvUOZxE3J2qsi0RmsfnZKN8DnKtQ==";
jQuery(document).ready(function ($) {
     jwplayer('my-video').setup({
		 file: 'https://mcnmedia.s3.eu-west-1.amazonaws.com/ec24837a244400d3f46a8d6d17189782f91bdd20/videos/ec24837a244400d3f46a8d6d17189782f91bdd20_672.stream_2020-10-15-12.17.01.471-UTC.mp4?X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&amp;X-Amz-Algorithm=AWS4-HMAC-SHA256&amp;X-Amz-Credential=AKIAJ3DCEPBRRJ3WJNFQ%2F20201021%2Feu-west-1%2Fs3%2Faws4_request&amp;X-Amz-Date=20201021T170234Z&amp;X-Amz-SignedHeaders=host&amp;X-Amz-Expires=1200&amp;X-Amz-Signature=5aee3c98e2042385d86c358288e0eddfd448ce824fcd69da6ac374527ed92715',
		image: "../css/jwerror.jpg",
        width: "100%",
     	aspectratio: "16:9",
		autostart: 'true',
		
		 //https://mcnmedia.s3.eu-west-1.amazonaws.com/ec24837a244400d3f46a8d6d17189782f91bdd20/videos/ec24837a244400d3f46a8d6d17189782f91bdd20_672.stream_2020-10-15-12.17.01.471-UTC.mp4?X-Amz-Content-Sha256=UNSIGNED-PAYLOAD&amp;X-Amz-Algorithm=AWS4-HMAC-SHA256&amp;X-Amz-Credential=AKIAJ3DCEPBRRJ3WJNFQ%2F20201021%2Feu-west-1%2Fs3%2Faws4_request&amp;X-Amz-Date=20201021T170234Z&amp;X-Amz-SignedHeaders=host&amp;X-Amz-Expires=1200&amp;X-Amz-Signature=5aee3c98e2042385d86c358288e0eddfd448ce824fcd69da6ac374527ed92715
		 //https://1502594353.rsc.cdn77.org/live/_ec24837a244400d3f46a8d6d17189782f91bdd20_/ec24837a244400d3f46a8d6d17189782f91bdd20_672.stream/playlist.m3u8
    });
	jwplayer().onError(function(){
	
	jwplayer().play();
	}); 

   // jwplayer("my-video").setup({
   //     sources: [{
			//file: "https://1502594353.rsc.cdn77.org/live/_ec24837a244400d3f46a8d6d17189782f91bdd20_/ec24837a244400d3f46a8d6d17189782f91bdd20_672.stream/playlist.m3u8"
   //     }, {
			//	file: "rtmp://1502594353.rsc.cdn77.org/live/_ec24837a244400d3f46a8d6d17189782f91bdd20_/ec24837a244400d3f46a8d6d17189782f91bdd20_672.stream"
   //     }],
   //     rtmp: {
   //         bufferlength: 3
   //     },
   //     fallback: true
   // });
	
	});
