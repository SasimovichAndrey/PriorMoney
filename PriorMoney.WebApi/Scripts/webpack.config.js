const path = require('path');

module.exports = {
    mode: 'development',
    entry: {
        Index: './src/viewscripts/Index.js'
    },
    output: {
        filename: '[name].js',
        path: __dirname + '/../wwwroot'
    }
};