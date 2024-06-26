﻿
export  const downloadFileFromStream = async (fileName, contentStreamReference) => {

//window.downloadFileFromStream = async (fileName, contentStreamReference) => {
    console.log("Start Download");
    const arrayBuffer = await contentStreamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? '';
    anchorElement.click();
    anchorElement.remove();
    URL.revokeObjectURL(url);
}


