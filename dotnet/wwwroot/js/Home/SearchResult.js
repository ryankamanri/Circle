let userContainer = document.querySelector(".user-container");
let searchResult = document.querySelector("#search-result").innerHTML;
let searchString = document.querySelector("input#search").value;

// userContainer.addEventListener("mousewheel",event => 
// {
//     event.preventDefault();
//     event.stopPropagation();
//     userContainer.scrollLeft += event.deltaY;
// });

userContainer.onmousewheel = event =>
{
    event.preventDefault();
    event.stopPropagation();
    userContainer.scrollLeft += event.deltaY;
}

//searchResult.innerText.replaceAll(searchString,`<span color="yellow">${searchString}</span>`);
let divMatchStrings = searchResult.match(/<div.*>.*<\/div>/g);
if(divMatchStrings != undefined) divMatchStrings.forEach(matchString => HignlightReplace(matchString));
let spanMatchStrings = searchResult.match(/<span.*>.*<\/span>/g);
if(spanMatchStrings != undefined) spanMatchStrings.forEach(matchString => HignlightReplace(matchString));
document.querySelector("#search-result").innerHTML = searchResult;

function HignlightReplace(matchString)
{
    let matchAttr = matchString.match(/>.*</g)[0];
    let matchAttrReplace = matchAttr.replaceAll(searchString,`<strong style="color: red">${searchString}</strong>`);
    let matchStringReplace = matchString.replace(matchAttr,matchAttrReplace);
    searchResult = searchResult.replace(matchString,matchStringReplace);
}





