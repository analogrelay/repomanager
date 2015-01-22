var gulp = require('gulp');
var bower = require('main-bower-files');
var sass = require('gulp-sass');
var clean = require('gulp-clean');

gulp.task('sass', function () {
    gulp.src('./client/styles/*.scss')
        .pipe(sass())
        .pipe(gulp.dest('./wwwroot/css'));
});

gulp.task('bower', function () {
    return gulp.src(bower(), { base: 'bower_components' })
               .pipe(gulp.dest("./wwwroot/lib/"));
});

gulp.task('clean', function () {
    return gulp.src('./wwwroot', {read: false})
               .pipe(clean());
});

gulp.task('copy', function () {
	return gulp.src('./client/scripts/*.js')
               .pipe(gulp.dest("./wwwroot/scripts/"));
});

gulp.task('default', ['bower', 'sass', 'copy']);
