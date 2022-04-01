var gulp = require('gulp');

var clean = require('gulp-clean');
gulp.task('clean', function () {
    return gulp.src('./public', { read: false, allowEmpty: true })
        .pipe(clean());
});

var pug = require('gulp-pug');
gulp.task('pug', function () {
    return gulp.src('./source/pug/pages/**/*.pug')
        .pipe(pug({ pretty: true }))
        .pipe(gulp.dest('./public'));
});

const sass = require('gulp-sass')(require('sass'));
const cleanCSS = require('gulp-clean-css');
gulp.task('sass', function () {
    return gulp.src('./source/sass/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCSS({compatibility: 'ie8'}))
        .pipe(gulp.dest('./public/stylesheets'));
});


gulp.task('js', function () 
{
    return gulp.src('./source/javascript/**/*.js')
        .pipe(gulp.dest('./public/scripts'));
});

gulp.task('images', function () 
{
    gulp.src('./source/images/favicon.ico').pipe(gulp.dest('./public'));

    return gulp.src('./source/images/*.*')
        .pipe(gulp.dest('./public/images'));
});

gulp.task('fonts', function () {
    return gulp.src('./source/fonts/**/*.*')
        .pipe(gulp.dest('./public/fonts'));
});

gulp.task('data', function () 
{
    return gulp.src('./source/data/*.*')
        .pipe(gulp.dest('./public/data'));
});


gulp.task('package', function () 
{
    return gulp.src('./public/**')
        .pipe(gulp.dest("./package"));
});

var del = require("del");
var rename = require("gulp-rename");
gulp.task('copysettings', function () 
{
    del("./package/scripts/settings.js");
    return gulp.src('./public/scripts/settings_live.js')
    .pipe(rename("settings.js"))
    .pipe(gulp.dest("./package/scripts", {overwrite: false}));
});

var password = process.env.FTP_PASSWORD; // System->Environmment Variables
var gutil = require( 'gulp-util' );
var ftp = require( 'vinyl-ftp' );
gulp.task( 'copytoftp', function () {
 
    var conn = ftp.create( {
        host:     'nt48.unoeuro.com',
        user:     'mjukvaror.com',
        password: password,
        parallel: 10,
        log:      gutil.log
    } );

    return gulp.src( './package/**', { buffer: false } )
        .pipe( conn.newer('/skanskastransformationspuls'))
        .pipe( conn.dest('/skanskastransformationspuls'));
 
} );

gulp.task('removepackage', function () {
    return gulp.src('./package', { read: false, allowEmpty: true })
        .pipe(clean());
});


gulp.task('watch', function () {
    gulp.watch('source/pug/**/*.pug', gulp.series('pug'));
    gulp.watch('source/sass/**/*.scss', gulp.series('sass'));
    gulp.watch('source/javascript/**/*.js', gulp.series('js'));
    gulp.watch('source/data/**/*.json', gulp.series('data'));
});

gulp.task('default', gulp.series('clean', 'pug', 'sass', 'js', 'images', 'fonts', 'data', function (done) {
    done();
}));


gulp.task('deploy', gulp.series('removepackage', 'clean', 'pug', 'sass', 'js', 'images', 'data', 'package', 'copysettings', 'copytoftp', 'removepackage', function (done) {
    done();
}));
