#!/usr/bin/env python3
# encoding=utf-8
import os
import mimetypes
import traceback
import subprocess
from subprocess import PIPE

from tqdm import tqdm


class Encoding:
    UTF8 = 'utf-8'
    UTF8_WITH_BOM = 'utf-8-sig'
    UTF16 = 'utf-16'
    GB2312 = 'gb2312'

    @classmethod
    def decode(cls, bs: bytes):
        try:
            return cls.UTF8_WITH_BOM, bs.decode(cls.UTF8_WITH_BOM)
        except Exception as ex:
            # traceback.print_exc()
            pass

        try:
            return cls.UTF8, bs.decode(cls.UTF8)
        except Exception as ex:
            # traceback.print_exc()
            pass

        try:
            return cls.UTF16, bs.decode(cls.UTF16)
        except Exception as ex:
            # traceback.print_exc()
            pass

        try:
            return cls.GB2312, bs.decode(cls.GB2312)
        except Exception as ex:
            # traceback.print_exc()
            pass

        return None, bs


skips = [
    '.git',
    'logs',
    'bin',
    'obj',
    '.vs',
]

skip_extensions = [
    '.dll',
    '.jpg',
    '.gif',
    '.png',
    '.bomb',
    '.map',
    '.suo',
    '.xls',
]


def find_all_files(infile):
    basename = os.path.basename(infile)
    if basename in skips:
        return []

    if 'Backup' in basename:
        return []

    retval = []

    if os.path.isfile(infile):
        ext = os.path.splitext(infile)[1].lower()
        if ext in skip_extensions:
            return []
        else:
            return [infile]

    elif os.path.isdir(infile):
        flist = os.listdir(infile)
        for fname in flist:
            fpath = f'{infile}/{fname}'
            retval.extend(find_all_files(fpath))

    return retval


if __name__ == '__main__':
    # all files
    # file_list = find_all_files('.')

    # tracked files only
    completed_process = subprocess.run(
        ['git', 'ls-files'],
        stdout=PIPE,
        stderr=PIPE,
    )

    lines = completed_process.stdout.decode('utf-8').split('\n')

    file_list = list(filter(lambda x: len(x) > 0, lines))

    pbar = tqdm(file_list)
    for fpath in pbar:
        pbar.set_description(fpath)
        # mime = mimetypes.guess_type(fpath)
        # print(mime, fpath)

        if not os.path.exists(fpath):
            continue

        basename = os.path.basename(fpath)
        ext = os.path.splitext(basename)[1].lower()
        if ext in skip_extensions:
            continue

        bs = open(fpath, mode='rb').read()
        encoding, decoded_string = Encoding.decode(bs)

        if encoding is None:
            continue

        if not encoding == Encoding.UTF8:
            open(fpath, mode='w', encoding=Encoding.UTF8).write(decoded_string)

        # enforce LF line ending
        lf_only = decoded_string.replace('\r\n', '\n')

        os.remove(fpath)  # file will not be changed if we don't remove it
        open(fpath, mode='w', encoding=Encoding.UTF8).write(lf_only)

        # print(encoding, fpath)
