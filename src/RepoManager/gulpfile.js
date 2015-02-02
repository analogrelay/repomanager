var gulp = require('gulp');
var bower = require('main-bower-files');
var sass = require('gulp-sass');
var clean = require('gulp-clean');
var react = require('gulp-react');

var paths = {
  styles: './client/styles/*.scss',
  scripts: './client/scripts/*.js'
};

gulp.task('sass', function () {
    gulp.src(paths.styles)
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
	return gulp.src(paths.scripts)
               .pipe(gulp.dest("./wwwroot/scripts/"));
});

gulp.task('watch', function() {
    gulp.watch(paths.styles, ['sass']);
    gulp.watch(paths.scripts, ['copy'])
});

gulp.task('react', function() {
	return gulp.src('./client/**/*.jsx')
		   .pipe(react())
		   .pipe(gulp.dest('./wwwroot/'));
});

gulp.task('default', ['bower', 'sass', 'copy']);
