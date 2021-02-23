var gulp = require('gulp');

var clean = require('gulp-clean');
gulp.task('clean', function () {
    return gulp.src('./public', { read: false, allowEmpty: true })
        .pipe(clean());
});

var pug = require('gulp-pug');
gulp.task('pug', function () {
    return gulp.src('./source/pug/pages/*.pug')
        .pipe(pug({ pretty: true }))
        .pipe(gulp.dest('./public'));
});

var sass = require('gulp-sass');
sass.compiler = require('node-sass');
const cleanCSS = require('gulp-clean-css');
gulp.task('sass', function () {
    return gulp.src('./source/sass/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(cleanCSS({compatibility: 'ie8'}))
        .pipe(gulp.dest('./public/stylesheets'));
});


gulp.task('js', function () 
{
    return gulp.src('./source/javascript/*.js')
        .pipe(gulp.dest('./public/scripts'));
});

gulp.task('images', function () 
{
    return gulp.src('./source/images/*.*')
        .pipe(gulp.dest('./public/images'));
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


var azure = require('gulp-azure-storage');
gulp.task('copytoazure', function () 
{
    return gulp.src('./package/**')
        .pipe(azure.upload({
    	    account:    'devopshealthradar',
    	    key:        'sDXKQBPu9Z9OQNXU5T2PYYXV+2wEhHkrrbIvPrDBBPb0rQFG9y75k8B+t2dHR8EExJtlyyA4h8PR4GpPldqkJA==',
    	    container:  '$web'
        }));
});

gulp.task('removepackage', function () {
    return gulp.src('./package', { read: false, allowEmpty: true })
        .pipe(clean());
});


gulp.task('watch', function () {
    gulp.watch('source/pug/**/*.pug', gulp.series('pug'));
    gulp.watch('source/sass/**/*.scss', gulp.series('sass'));
    gulp.watch('source/javascript/**/*.js', gulp.series('js'));
});

gulp.task('default', gulp.series('clean','pug','sass','js','images', function (done) {
    done();
}));


gulp.task('deploy', gulp.series('removepackage','clean','pug','sass','js','images' ,'package','copysettings','copytoazure','removepackage', function (done) {
    done();
}));
