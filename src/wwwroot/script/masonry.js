window.initMasonry = (parentSelector, itemSelector, columnWidth, percentPosition, transitionDuration) => {
    var masonry = new Masonry(parentSelector, {
        itemSelector: itemSelector,
        columnWidth: columnWidth,
        percentPosition: percentPosition,
        transitionDuration: transitionDuration
    });
};