# News-Articles
This module was originally created by Scott McCulloch of Ventrian. The module is now out in the open, and mainly maintained  by Stefan Kamphuis, Timo Breumelhof and Peter Schotman of [40FINGERS](https://www.40fingers.net/). \
We are mainly fixing issues for our clients, so don't expect 40F to fix all issues. \
Pull request are always welcome. :-) \
We are currently in the process of converting our clients News Articles modules to [Open Content](https://opencontent.readme.io/) and will stop using and supporting this module in the near future.


## Installation

Install the module through Host -> Extensions.

Ready made install files are located in the /installs folder.

## Token Guide

### Menu.Item.Html

	[ADMINLINK]
	[ARCHIVESLINK]
	[APPROVEARTICLESLINK]
	[APPROVECOMMENTSLINK]
	[CATEGORIESLINK]
	[CURRENTARTICLESLINK]
	[HASCOMMENTSENABLED][/HASCOMMENTSENABLED]
	[ISADMIN][/ISADMIN]
	[ISAPPROVER][/ISAPPROVER]
	[ISSELECTEDADMIN][/ISSELECTEDADMIN]
	[ISSELECTEDAPPROVEARTICLES][/ISSELECTEDAPPROVEARTICLES]
	[ISSELECTEDAPPROVECOMMENTS][/ISSELECTEDAPPROVECOMMENTS]
	[ISSELECTEDCATEGORIES][/ISSELECTEDCATEGORIES]
	[ISSELECTEDCURRENTARTICLES][/ISSELECTEDCURRENTARTICLES]
	[ISSELECTEDMYARTICLES][/ISSELECTEDMYARTICLES]
	[ISSELECTEDSEARCH][/ISSELECTEDSEARCH]
	[ISSELECTEDSYNDICATION][/ISSELECTEDSYNDICATION]
	[ISSELECTEDSUBMITARTICLE][/ISSELECTEDSUBMITARTICLE]
	[ISSYNDICATIONENABLED][/ISSYNDICATIONENABLED]
	[ISSUBMITTER][/ISSUBMITTER]
	[MYARTICLESLINK][/MYARTICLESLINK]
	[RSSLATESTLINK][/RSSLATESTLINK]
	[SEARCHLINK][/SEARCHLINK]
	[SUBMITARTICLELINK][/SUBMITARTICLELINK]
	[SYNDICATIONLINK][/SYNDICATIONLINK]
	[RESX:XXX] where XXX is the name of the key in sharedresources.ascx.resx
 
### Listing.Header.Html/Listing.Footer.Html

	[CATEGORYFILTER]
	[CATEGORYSELECTED][/CATEGORYSELECTED]
	[CATEGORYNOTSELECTED][/CATEGORYNOTSELECTED]
	[CURRENTPAGE]
	[HASMULTIPLEPAGES][/HASMULTIPLEPAGES]
	[HASNEXTPAGE][/HASNEXTPAGE]
	[HASPREVPAGE][/HASPREVPAGE]
	[LINKPREVIOUS]
	[LINKPREVIOUSURL]
	[LINKNEXT]
	[LINKNEXTURL]
	[PAGECOUNT]
	[TABID]

### Listing.Featured.Html/Listing.Item.Html/View.Item.Html/View.Title.Html/View.Description.Html/View.Keyword.Html/Handout.Item.Html/Rss.Item.Html

	[ARTICLEID]
	[ARTICLELINK]
	[ARTICLELINK:XXX] where XXX is antoher article ID.
	[APPROVERDISPLAYNAME]
	[APPROVERFIRSTNAME]
	[APPROVERLASTNAME]
	[APPROVERUSERNAME]
	[AUTHOR]
	[AUTHOR:XXX] where XXX is the profile field name. e.g. [AUTHOR:FirstName]
	[AUTHOREMAIL]
	[AUTHORUSERNAME]
	[AUTHORFIRSTNAME]
	[AUTHORLASTNAME]
	[AUTHORFULLNAME]
	[AUTHORID]
	[CAPTION:XXX] where XXX is the name of the caption.
	[CATEGORIES]
	[CATEGORIESNOLINK]
	[CREATEDATE]
	[CREATETIME]
	[COMMENTCOUNT]
	[COMMENTLINK]
	[COMMENTRSS]
	[COMMENTS] - loads the comment.item template for each comment.
	[CURRENTPAGE]
	[CUSTOMFIELDS]
	[CUSTOM:XXX] where XXX is the name of the custom field.
	[DETAILS]
	[DETAILS:XXX] where XXX is the number of characters to show.
	[EDIT]
	[EXPRESSION:XXX:YY:ZZZ][/EXPRESSION:XXX:YY:ZZZ] where XXX is the name of a custom field, YY is the operator (=,!=,>,>=,<,<=) and ZZZ is the value to compare against. e.g. [EXPRESSION:Bedrooms:>:5]Wow this is a big place[/EXPRESSION:Bedrooms:>:5]
	[FILECOUNT]
	[FILES] -- Loads file templates for each file
	[GRAVATARURL]
	[HASAUTHOR][/HASAUTHOR]
	[HASAUTHORVALUE:XXX][/HASAUTHORVALUE:XXX]
	[HASCOMMENTS][/HASCOMMENTS]
	[HASCOMMENTSENABLED][/HASCOMMENTSENABLED]
	[HASCUSTOMFIELDS][/HASCUSTOMFIELDS]
	[HASFILES][/HASFILES]
	[HASIMAGE][/HASIMAGE]
	[HASIMAGES][/HASIMAGES]
	[HASMULTIPLEIMAGES][/HASMULTIPLEIMAGES] - image count > 1
	[HASNOAUTHOR][/HASNOAUTHOR]
	[HASNOCOMMENTS][/HASNOCOMMENTS]
	[HASNOFILES][/HASNOFILES]
	[HASNOIMAGE][/HASNOIMAGE]
	[HASNOIMAGES][/HASNOIMAGES]
	[HASNOLINK][/HASNOLINK]
	[HASLINK][/HASLINK]
	[HASMOREDETAIL][/HASMOREDETAIL]
	[HASMOREDETAIL:XXX][/HASMOREDETAIL:XXX] where XXX is the number of characters to check against.
	[HASMULTIPLEPAGES][/HASMULTIPLEPAGES]
	[HASNEXTPAGE][/HASNEXTPAGE]
	[HASPREVPAGE][/HASPREVPAGE]
	[HASRATING][/HASRATING]
	[HASRATINGSENABLED][/HASRATINGSENABLED]
	[HASRELATED][/HASRELATED]
	[HASSUMMARY][/HASSUMMARY]
	[HASNOSUMMARY][/HASNOSUMMARY]
	[HASTAG:XXX][/HASTAG:XXX] where XXX is the name of the tag.
	[HASVALUE:XXX][/HASVALUE:XXX] where XXX is the name of a custom field.
	[HASNOVALUE:XXX][/HASNOVALUE:XXX] where XXX is the name of a custom field.
	[HASTAGS][/HASTAGS]
	[IMAGE]
	[IMAGE:ZZZ] (Where ZZZ is the position in list of images)
	[IMAGECOUNT]
	[IMAGELINK]
	[IMAGES] -- loads images templates
	[IMAGETHUMB:XXX:YYY] (Where XXX is the maximum width and YYY is the maximum height)
	[IMAGETHUMBRANDOM:XXX:YYY] (Where XXX is the maximum width and YYY is the maximum height)
	[IMAGETHUMB:XXX:YYY:ZZZ] (Where XXX is the maximum width and YYY is the maximum height and ZZZ is the position in list of images)
	[IMAGETHUMBLINK:XXX] is the actual URL of the link to the thumbnail image.
	[ISANONYMOUS][/ISANONYMOUS]
	[ISAUTHOR][/ISAUTHOR]
	[ISDRAFT][/ISDRAFT]
	[ISFEATURED][/ISFEATURED]
	[ISNOTFEATURED][/ISNOTFEATURED]
	[ISFIRST][/ISFIRST]
	[ISFIRST2][/ISFIRST2] only true when on the first page of listings.
	[ISINROLE:XXX][/ISINROLE:XXX] where XXX is the role name.
	[ISITEMINDEX:XXX][/ISITEMINDEX:XXX] where XXX is a number.
	[ISLOCALE:XXX][/ISLOCALE:XXX]
	[ISNOTANONYMOUS][/ISNOTANONYMOUS]
	[ISNOTFIRST][/ISNOTFIRST]
	[ISNOTSECOND][/ISNOTSECOND]
	[ISNOTSECURE][/ISNOTSECURE]
	[ISPAGE:XXX][/ISPAGE:XXX]
	[ISNOTPAGE:XXX][/ISNOTPAGE:XXX]
	[ISPUBLISHED][/ISPUBLISHED]
	[ISRATEABLE][/ISRATEABLE]
	[ISRSSITEM][/ISRSSITEM]
	[ISNOTRSSITEM][/ISNOTRSSITEM]
	[ISSECOND][/ISSECOND]
	[ISSECURE][/ISSECURE]
	[ISSYNDICATIONENABLED][/ISSYNDICATIONENABLED]
	[ITEMINDEX]
	[LASTUPDATEDATE]
	[LASTUPDATEEMAIL]
	[LASTUPDATEFIRSTNAME]
	[LASTUPDATELASTNAME]
	[LASTUPDATEUSERNAME]
	[LASTUPDATEFULLNAME]
	[LASTUPDATEID]
	[LINK]
	[LINKNEXT]
	[LINKPREVIOUS]
	[LINKTARGET]
	[MODULEID]
	[PAGECOUNT]
	[PAGE:XX] where XX is the page number.
	[PAGER]
	[PAGER2] - an UL list of pages.
	[PAGES] (View.Item.Html Only)
	[PAGESLIST]
	[PAGESLIST2]
	[PAGETEXT]
	[PAGETITLE]
	[PAGETITLENEXT]
	[PAGETITLEPREV]
	[PORTALALIAS]
	[PORTALROOT]
	[POSTCOMMENT] - the post comment form.
	[POSTRATING] - the post rating form.
	[PRINT]
	[PRINTLINK]
	[PUBLISHSTARTDATE]
	[PUBLISHSTARTDATE:XXX] where XXX is a date format expression for flexibility of date format.
	[PUBLISHENDDATE]
	[PUBLISHENDDATE:XXX] where XXX is a date format expression for flexibility of date format.
	[RATING]
	[RATINGCOUNT]
	[RATINGDETAIL]
	[RELATED] (uses related templates, shows up to 5 articles).
	[RELATED:XXX] where XXX is the number of related articles to show (uses related templates)
	[SITETITLE]
	[SUMMARY]
	[TABID]
	[TABTITLE]
	[TAGS]
	[TAGSNOLINK]
	[TEMPLATEPATH]
	[TITLE]
	[TITLEURLENCODED]
	[TITLE:XXX] where XXX is the number of chars.
	[TWITTERNAME]
	[VIEWCOUNT]
	[SUMMARY:XXX] where XXX is a number of chars.
	[CREATEDATELESSTHAN:XXX] where XXX is number of days
	[UPDATEDATELESSTHAN:XXX] where XXX is number of days
	
### Image.Item.Html

	[ARTICLEID]
	[DESCRIPTION]
	[FILENAME]
	[HEIGHT]
	[IMAGEID]
	[IMAGELINK]
	[ISITEMINDEX:XXX][/ISITEMINDEX:XXX] -- where XXX is the item position number, e.g. 1
	[ISNOTITEMINDEX:XXX][/ISNOTITEMINDEX:XXX] -- where XXX is the item position number, e.g. 2
	[ITEMINDEX]
	[SIZE]
	[SORTORDER]
	[TITLE]
	[WIDTH]
	[IMAGETHUMB:XXX:YYY] where XXX is the max width and YYY is the max height.

### Comment.Item.Html/Rss.Comment.Item.Html/Latest Comments

	[ANONYMOUSURL]
	[ARTICLEID]
	[ARTICLELINK]
	[AUTHOR]
	[AUTHOR:XXX] where XXX is the profile field name. e.g. [AUTHOR:FirstName]
	[AUTHORDISPLAYNAME]
	[AUTHOREMAIL]
	[AUTHORUSERNAME]
	[AUTHORFIRSTNAME]
	[AUTHORLASTNAME]
	[AUTHORFULLNAME]
	[AUTHORID]
	[COMMENTID]
	[COMMENT]
	[COMMENT:XX] where XXX is the maximum number of characters to show (will remove html)
	[CREATEDATE]
	[CREATEDATE:XXX] where XXX is the date format expression.
	[CREATETIME]
	[DELETE]
	[GRAVATAR]
	[GRAVATARURL]
	[HASANONYMOUSURL][/HASANONYMOUSURL]
	[IPADDRESS]
	[ISANONYMOUS][/ISANONYMOUS]
	[ISNOTANONYMOUS][/ISNOTANONYMOUS]
	[ISAUTHOR][/ISAUTHOR]
	[ISCOMMENT][/ISCOMMENT]
	[ISINROLE:XXX][/ISINROLE:XXX] (where XXX is the name of a role)
	[ISPINGBACK][/ISPINGBACK]
	[ISTRACKBACK][/ISTRACKBACK]
	[ITEMINDEX]
	[MODULEID]
	[PINGBACKURL]
	[RATING]
	[TRACKBACKBLOGNAME]
	[TRACKBACKEXCERPT]
	[TRACKBACKTITLE]
	[TRACKBACKURL]

### Rss.Item.Html (As well as the tokens for View.Item.Html)

	[DESCRIPTION]
	[ENCLOSURELENGTH]
	[ENCLOSURELINK]
	[ENCLOSURETYPE]
	[GUID]
	[HASENCLOSURE][/HASENCLOSURE]
	[TRACKBACKLINK]

Rss.Header.Html/Rss.Footer.Html

	[PORTALNAME]
	[PORTALURL]

### Category.Html (shown at the top of the view category page)

	[ARTICLECOUNT]
	[CATEGORYLABEL]
	[CATEGORYID]
	[CHILDCATEGORIES] - loads the child categories 1 level deep (loads Category.Child.Html for each)
	[DESCRIPTION]
	[HASCHILDCATEGORIES][/HASCHILDCATEGORIES]
	[HASNOCHILDCATEGORIES][/HASNOCHILDCATEGORIES]
	[HASNOPARENT][/HASNOPARENT]
	[HASPARENT][/HASPARENT]
	[LINK]
	[NAME]
	[PARENTDESCRIPTION]
	[PARENTLINK]
	[PARENTNAME]
	[RSSLINK]
	[VIEWS]
	[CHILDCATEGORIES:XXX] where XXX is the number of levels deep.
	[DESCRIPTION:XXX] where XXX is the number of characters to display.
	[ISDEPTHABS:XXX][/ISDEPTHABS:XXX] where XXX is the number of levels deep from the root node.
	[ISNOTDEPTHABS:XXX][/ISNOTDEPTHABS:XXX] where XXX is the number of levels deep from the root node.
	[PARENTDESCRIPTION:XXX] where XXX is the number of characters to display.

### Category.Child.Html

	[ARTICLECOUNT]
	[CATEGORYID]
	[DEPTHABS] - The depth of the category from the root category.
	[DEPTHREL] - The depth of the category in relation to the current category.
	[DESCRIPTION]
	[LINK]
	[NAME]
	[RSSLINK]
	[ORDER]
	[VIEWS]
	[DESCRIPTION:XXX] where XXX is the number of characters to display.
	[IFORDER:XXX][/IFORDER:XXX] where XXX is the sort order number.
	[IFNOTORDER:XXX][/IFNOTORDER:XXX] where XXX is the sort order number.
	[ISDEPTHABS:XXX][/ISDEPTHABS:XXX] where XXX is the depth of the category from the root category.
	[ISDEPTHREL:XXX][/ISDEPTHREL:XXX] where XXX is the depth of the category relative to the current node.
	[ISNOTDEPTHABS:XXX][/ISNOTDEPTHABS:XXX] where XXX is the depth of the category from the root category.
	[ISNOTDEPTHREL:XXX][/ISNOTDEPTHREL:XXX] where XXX is the depth of the category relative to the current node.

### File.Item.Html

	[ARTICLEID]
	[FILEID]
	[FILENAME]
	[FILELINK]
	[SIZE]
	[SORTORDER]
	[TITLE]
	[ISEXTENSION:XXX][/ISEXTENSION:XXX] where XXX is the extension name.
	[ISNOTEXTENSION:XXX][/ISNOTEXTENSION:XXX] where XXX is the extension name.

### News Archives (Date)

	[COUNT]
	[ISSELECTEDMONTH][/ISSELECTEDMONTH]
	[ISNOTSELECTEDMONTH][/ISNOTSELECTEDMONTH]
	[ISSELECTEDYEAR][/ISSELECTEDYEAR]
	[ISNOTSELECTEDYEAR][/ISNOTSELECTEDYEAR]
	[LINK]
	[MONTH]
	[YEAR]

### News Archives (Category)

	[DEPTHABS]
	[CATEGORY]
	[CATEGORYNOTINDENTED]
	[COUNT]
	[DEPTHREL]
	[LINK]

### News Archives (Author)

	[AUTHORID]
	[AUTHORUSERNAME]
	[AUTHORDISPLAYNAME]
	[AUTHORFIRSTNAME]
	[AUTHORLASTNAME]
	[AUTHORFULLNAME]
	[COUNT]
	[LINK]

### Handout.Cover.Html, Handout.End.Html, Handout.Header.Html, Handout.End.Html

	[DESCRIPTION]
	[LOGO]
	[NAME]
 

