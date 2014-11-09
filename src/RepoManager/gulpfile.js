var gulp = require('gulp');
var bower = require('gulp-bower');

gulp.task('bower', function () {
    return bower()
      .pipe(gulp.dest('wwwroot/lib/'))
});

gulp.task('default', ['bower'], function () {
    //tcopy font-awesome without a plugin
    gulp.src("./bower_components/font-awesome/fonts/**/*.{ttf,woff,eof,svg}")
        .pipe(gulp.dest("./wwwroot/fonts"));
});