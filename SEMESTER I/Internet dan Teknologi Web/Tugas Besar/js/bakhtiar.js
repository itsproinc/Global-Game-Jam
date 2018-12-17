objectFitVideos(); // Load objectfitvideos plugin

const sideNav = document.querySelectorAll('.sidenav');
M.Sidenav.init(sideNav);

const slider = document.querySelectorAll('.slider');
M.Slider.init(slider, {
    indicators: false,

    height: 500,
    transition: 700,
    interval: 3000
});

const parallax = document.querySelectorAll('.parallax');
M.Parallax.init(parallax);

window.onscroll = function() {UpdatePage()};
window.onloadstart = function() {UpdatePage()};  // Load UpdatePage() on start 

var header = document.getElementById("header");
function UpdatePage() {
    console.log($.scrollify.currentIndex());

    // Navbar
    if(window.pageYOffset > header.offsetTop)
    {
        if(!header.classList.contains("fadein"))
            header.classList.add("fadein");
        else
            header.addEventListener("transitionend", DeleteClass());  
    }              
    else if(header.classList.contains("fadein"))
        header.classList.add("fadeout");  

    // Sections
    // About Us
    var aboutus = document.getElementById("aboutus");     
    if ($('#aboutus_sect').visible(true))
    {
        if(!aboutus.classList.contains("aboutUs_sectionfadein"))
            aboutus.classList.add("aboutUs_sectionfadein");

        aboutus.addEventListener("animationend", CheckAboutUsAnim()); 
    }
    if (($('#aboutus_sect').visible(true)) === false && aboutus.classList.contains("aboutUs_sectionfadein"))
        aboutus.classList.add("aboutUs_sectionfadeout"); 

    // Our Games
    var ourgames = document.getElementById("ourgames");                 
    if ($('#ourgames_sect').visible(true))
    {
        if(!ourgames.classList.contains("ourGames_sectionfadein"))
            ourgames.classList.add("ourGames_sectionfadein");

        ourgames.addEventListener("animationend", CheckOurGamesAnim()); 
    }            
    if (($('#ourgames_sect').visible(true)) === false && ourgames.classList.contains("ourGames_sectionfadein"))
        ourgames.classList.add("ourGames_sectionfadeout"); 

    // Developers
    var developers = document.getElementById("developers");           
    if ($('#developers_sect').visible(true))
    {
        if(!developers.classList.contains("developers_sectionfadein"))
            developers.classList.add("developers_sectionfadein");
        developers.addEventListener("animationend", CheckDevelopers());
    }        
    if (($('#developers_sect').visible(true)) === false && developers.classList.contains("developers_sectionfadein"))
        developers.classList.add("developers_sectionfadeout"); 

    // Contact us
    var contactus = document.getElementById("contactus");                 
    if ($('#contactus_sect').visible(true))
    {
        if(!contactus.classList.contains("contactUs_sectionfadein"))
            contactus.classList.add("contactUs_sectionfadein");
        contactus.addEventListener("animationend", CheckContactUs());
    }          
    if (($('#contactus_sect').visible(true)) === false && contactus.classList.contains("contactUs_sectionfadein"))
        contactus.classList.add("contactUs_sectionfadeout");     
}

// Movement (scrollspy)
$.scrollify({
    easing: "easeOutExpo",
    scrollSpeed: 1500,
    offset : -55,
    scrollbars: true,
    setHeights: false,
    overflowScroll: true,
    touchScroll: true
});

$('.home_btn').click(function(){
    if($.scrollify.currentIndex() != 0)
        $.scrollify.move("#1");
}); 

$('.aboutus_btn').click(function(){  
    if($.scrollify.currentIndex() != 1)
        $.scrollify.move("#2");
});

$('.ourgames_btn').click(function(){  
    if($.scrollify.currentIndex() != 2)
        $.scrollify.move("#3");
});

$('.developers_btn').click(function(){  
    if($.scrollify.currentIndex() != 3)
        $.scrollify.move("#4");
});

$('.contactus_btn').click(function(){
    if($.scrollify.currentIndex() != 4)
        $.scrollify.move("#5");
});

$(function() {
  $.scrollify({
    section : ".scrollspy"
  });
});

// Nav
window.ontransitionend = function() {DeleteClass()};
function DeleteClass() {
    if(header.classList.contains("fadein") && header.classList.contains("fadeout"))
    {
        header.classList.remove("fadein");
        header.classList.remove("fadeout");
        UpdatePage();
    }
}

// About Us
var AboutUsAnim = false;        
function CheckAboutUsAnim () {
    if(!AboutUsAnim)
    {
        AboutUsAnim = true;
        if(aboutus.classList.contains("aboutUs_sectionfadein") && aboutus.classList.contains("aboutUs_sectionfadeout"))
        {
            aboutus.classList.remove("aboutUs_sectionfadein");
            aboutus.classList.remove("aboutUs_sectionfadeout");    
            AboutUsAnim = false;     
        }
        else
            AboutUsAnim = false;
    }
}

// Our Games
var OurGamesAnim = false;        
function CheckOurGamesAnim () {
    if(!OurGamesAnim)
    {
        OurGamesAnim = true;
        if(ourgames.classList.contains("ourGames_sectionfadein") && ourgames.classList.contains("ourGames_sectionfadeout"))
        {
            ourgames.classList.remove("ourGames_sectionfadein");
            ourgames.classList.remove("ourGames_sectionfadeout");
            OurGamesAnim = false;           
        }
        else
        OurGamesAnim = false;
    }
}

// Developers
var DevelopersAnim = false;        
function CheckDevelopers () {
    if(!DevelopersAnim)
    {
        DevelopersAnim = true;
        if(developers.classList.contains("developers_sectionfadein") && developers.classList.contains("developers_sectionfadeout"))
        {
            developers.classList.remove("developers_sectionfadein");
            developers.classList.remove("developers_sectionfadeout");      
            DevelopersAnim = false;     
        }
        else
        DevelopersAnim = false;
    }
}

// Contact Us
var ContactUsAnim = false;        
function CheckContactUs () {
    if(!ContactUsAnim)
    {
        ContactUsAnim = true;
        if(contactus.classList.contains("contactUs_sectionfadein") && contactus.classList.contains("contactUs_sectionfadeout"))
        {
            contactus.classList.remove("contactUs_sectionfadein");
            contactus.classList.remove("contactUs_sectionfadeout");  
            ContactUsAnim = false;         
        }
        else
        ContactUsAnim = false;
    }
}