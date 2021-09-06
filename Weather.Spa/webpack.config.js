const HtmlWebPackPlugin = require("html-webpack-plugin");
const path = require('path');
module.exports = {
  entry: path.resolve('./src/index.js'),
  output: {
      path: path.resolve('./dist'),
      filename: 'bundle.js', 
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: {
          loader: "babel-loader"
        }
      },
      {
        test: /\.html$/,
        use: [
          {
            loader: "html-loader"
          }
        ]
      },
      {
        test: /\.css$/,  
        exclude: /node_modules/,  
        use: [
                'style-loader',
                {
                   loader: 'css-loader',
                   options: {
                    modules: true,
                   }
                }
        ]
       }
    ]
  },
  plugins: [
    new HtmlWebPackPlugin({
      template: "./src/index.html",
      filename: "./index.html"
    })
  ]
};
