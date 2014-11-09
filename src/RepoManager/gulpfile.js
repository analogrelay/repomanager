var gulp = require('gulp');
var bower = require('gulp-bower');
var sass = require('gulp-sass');

gulp.task('sass', function () {
    gulp.src('./client/styles/*.scss')
        .pipe(sass())
        .pipe(gulp.dest('./wwwroot/css'));
});

gulp.task('bower', function () {
    return bower()
      .pipe(gulp.dest('wwwroot/lib/'))
});

gulp.task('default', ['bower', 'sass'], function () {
    //tcopy font-awesome without a plugin
    gulp.src("./bower_components/font-awesome/fonts/**/*.{ttf,woff,eof,svg}")
        .pipe(gulp.dest("./wwwroot/fonts"));
});